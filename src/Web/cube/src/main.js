import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import ViewUI from 'view-design';
import 'view-design/dist/styles/iview.css';
import axios from 'axios';
import canvas from 'html2canvas'
import vueParticles from 'vue-particles';
import VueConfirmDialog from 'vue-confirm-dialog'

Vue.config.productionTip = false
Vue.use(router);
Vue.use(canvas);
Vue.use(vueParticles);
Vue.use(VueConfirmDialog)
Vue.component('vue-confirm-dialog', VueConfirmDialog.default)

Vue.prototype.axios = axios;

Vue.use(ViewUI, {
    transfer: true,
    size: 'default',
});

axios.interceptors.response.use(
    response => {
        return response
    },
    error => {
        if (error.response) {
            if (error.response.status == 401) {

                console.log("Redirect to Login, Error Detail:" + error.response);
                localStorage.setItem('LOGINUSER', '');
                localStorage.setItem('TOKEN', '');

                router.replace('/login');
            }
            return Promise.reject(error)
        }
    }
);

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app')


