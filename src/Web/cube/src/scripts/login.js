export default {
    name:"login",
    data() {
        return {
            formInline: {
                user: '',
                password: ''
            },
            ruleInline: {
                user: [
                    { required: true, message: 'Please fill in your user name', trigger: 'blur' }
                ],
                password: [
                    { required: true, message: 'Please fill in your password.', trigger: 'blur' },
                    { type: 'string', min: 6, message: 'The password length cannot be less than 6 bits', trigger: 'blur' }
                ]
            }
        }
    },
    methods: {
        handleSubmit(name) {
            this.$refs[name].validate((valid) => {
                if (valid) {

                    this.axios({
                        method: 'post',
                        url: '/Authorize',
                        data: {
                            userName: this.formInline.user,
                            password: this.formInline.password
                        },
                    }).then(res => {

                        localStorage.setItem('LOGINUSER', this.formInline.user);
                        localStorage.setItem('TOKEN', res.data.access_token); 

                        this.$Message.success('Hi ' + this.formInline.user + ', Welcome to Cube System!');
                        this.$router.replace({ path: '/board', params: { username: this.formInline.user } } );

                    }).catch(error => {
                        this.$Message.error('Username or Password is incorrect, please try again!');
                        console.log(error);
                    })
                } else {
                    //this.$Message.error('Fail!');
                }
            })
        }
    }
}