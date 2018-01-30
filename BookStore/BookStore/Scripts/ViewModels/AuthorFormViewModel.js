function AuthorFormViewModel() {
    var self = this;

    //使用ko.observable将self的三个属性均设置为监控变量，knockout就会开始跟踪它们的变化。
    self.saveCompleted = ko.observable(false);//可监控变量，初始值为false。用于在author数据保存成功时进行页面提示。
    self.sending = ko.observable(false);//用于在使用Ajax保存author时显示进度条，并隐藏提交按钮。

    self.isCreating = author.id == 0;

    self.author = {
        id: author.id,
        firstName: ko.observable(),
        lastName: ko.observable(),
        biography: ko.observable()
    };

    /*
    如果没有通过jQuery验证，就不提交表单；在Ajax请求中动态添加一个防替换令牌；通过Ajax表单POST请求，发送author对象。
    */
    self.validateAndSave = function (form) {
        if (!$(form).valid()) return false;

        self.sending(true);

        //include the anti forgery token
        self.author.__RequestVerificationToken = form[0].value;

        $.ajax({
            url: (self.isCreating)?'Create':'Edit',
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded',
            data: ko.toJS(self.author), //ko.toJS()，将viewmodel转换为JSON对象；ko.toJSON()，将viewmodel转换为序列化过的Json string；
            success: self.successfulSave,
            error: self.errorSave,
            complete: function () {
                self.sending(false);
            }
        });
    };

    self.successfulSave = function () {
        self.saveCompleted(true);
        $('.body-content').prepend(
            '<div class="alert alert-success"><strong>Success!</strong> The new author has been saved.</div>');
        setTimeout(function () {
            if (self.isCreating) {
                location.href = './'
            } else {
                location.href = '../';
            }
        }, 1000);
    };

    self.errorSave = function () {
        $('.body-content').prepend(
            '<div class="alert alert-danger"><strong>Error!</strong> There was an error saving the authro.</div>');
    };
            
}