export default {
    name: "register",
    data() {
        const pwdAgainCheck = async (rule, value, callback) => {
            if (value.length < 1) {
                this.changeAgainFlag = 2;
                return callback(new Error('confirm password can not be null！'));
            } else if (this.formInline.password != this.formInline.confirmPassword) {
                this.changeAgainFlag = 2;
                return callback(new Error('the password is not consistent！'));
            } else {
                this.changeAgainFlag = 1;
                callback()
            }
        };
        return {
            formInline: {
                user: '',
                password: '',
                confirmPassword: ''
            },
            ruleInline: {
                user: [
                    { required: true, message: 'Please fill in the user name', trigger: 'blur' }
                ],
                password: [
                    { required: true, message: 'Please fill in the password.', trigger: 'blur' },
                    { type: 'string', min: 6, message: 'The password length cannot be less than 6 bits', trigger: 'blur' }
                ],
                confirmPassword: [
                    { required: true, message: 'Please fill in the password.', trigger: 'blur' },
                    { type: 'string', min: 6, message: 'The password length cannot be less than 6 bits', trigger: 'blur', validator: pwdAgainCheck }

                ]
            }
        }
    },
    methods: {
        register(name) {
            this.$refs[name].validate((valid) => {
                if (valid) {
                    this.axios({
                        method: 'post',
                        url: '/User/Register',
                        data: {
                            Name: this.formInline.user,
                            Password: this.formInline.password
                        },
                    }).then(() => {
                        this.$Message.success(this.formInline.user + 'register success!');
                        this.$router.replace('/login');
                    }).catch(() => {
                        this.$Message.error('register failed!');
                    })
                } else {
                    this.$Message.error('Fail!');
                }
            })

        },
        
    }


}