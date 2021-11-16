import axios from 'axios';
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
                    { required: true, message: 'Please fill in the user name', trigger: 'blur' }
                ],
                password: [
                    { required: true, message: 'Please fill in the password.', trigger: 'blur' },
                    { type: 'string', min: 6, message: 'The password length cannot be less than 6 bits', trigger: 'blur' }
                ]
            }
        }
    },
    methods: {
        handleSubmit(name) {
            this.$refs[name].validate((valid) => {
                if (valid) {
                    axios({
                        method: 'get',
                        url: '/User',
                        data: this.formInline
                    }).then(res => {
                        console.log(res.data);
                        //_this.userToken = 'test ' + res.data[0].name;
                        //  _this.changeLogin({ Authorization: _this.userToken });
                        this.$Message.success('Success!');
                        this.$router.replace('/board');
                    }).catch(error => {
                        alert('username or password error');
                        console.log(error);
                    })


                } else {
                    this.$Message.error('Fail!');
                }
            })
        }
    }


}