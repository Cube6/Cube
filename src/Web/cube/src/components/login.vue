<template>
    <div class="post">
        <input type="text" v-model="loginForm.username" placeholder="用户名" />
        <input type="text" v-model="loginForm.password" placeholder="密码" />
        <button @click="login">登录</button>
    </div>
</template>

<script lang="js">
    import axios from 'axios';
    import { mapMutations } from 'vuex';

    export default{
        data() {
            return {
                loginForm: {
                    username: '',
                    password: ''

                }
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.login();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'login'
        },
        methods: {
            ...mapMutations(['changeLogin']),

            login()
            {
                let _this = this;
                if (this.loginForm.username === '' || this.loginForm.password === '') {
                    alert('username or password can not empty');
                }
                else {
                    axios({
                        method: 'get',
                        url: '/User',
                        data: _this.loginForm
                    }).then(res => {
                        console.log(res.data);
                        _this.userToken = 'test ' + res.data[0].name;
                        _this.changeLogin({ Authorization: _this.userToken });
                        _this.$router.push('/board');
                        alert('login success');
                    }).catch(error => {
                        alert('username or password error');
                        console.log(error);
                    })
                }
             }
        },
    };
</script>


