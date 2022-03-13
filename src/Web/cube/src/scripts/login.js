export default {
    name:"login",
    data() {
        return {
            loading: false,
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
    created(){
        var userToken = localStorage.getItem('TOKEN');
        var userName = localStorage.getItem('LOGINUSER');
        if(userToken != null && userName !=null)
        {
            console.log("Redirect to board view since the user is already logged in");
            this.$router.replace({ path: '/board', params: { username: userName } } );
        }
    },
    methods: {
        handleSubmit(name) {
            this.$refs[name].validate((valid) => {
                if (valid) {

                    this.loading = true;

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
                        this.loading = false;
                    })
                } else {
                    this.$Message.error('Fail!');
                }
            })
        },
        directToRegister() {
            this.$router.replace('/register');
        }
    }
}