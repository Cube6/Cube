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
            loading: false,
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

                    this.loading = true;

                    this.axios({
                        method: 'post',
                        url: '/User/Register',
                        data: {
                            Name: this.formInline.user,
                            Password: this.formInline.password
                        },
                    }).then((res) => {
                        if (res.data.Success) {
                            this.$Message.success('Created the account \'' + this.formInline.user + '\' successfully!');
                            this.$router.replace('/login');
                        }
                        else {
                            this.$Message.error('Failed to create your account!');
                            this.loading = false;
                        }   
                    }).catch(() => {
                        this.$Message.error('Failed to create your account!');

                        this.loading = false;
                    })
                } else {
                    this.$Message.error('Fail!');
                }
            })

        },
        redirectToLogin() {
            this.$router.replace('/login');
        }
    }
}