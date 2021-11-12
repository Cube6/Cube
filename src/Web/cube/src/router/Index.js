import Vue from 'vue'
import Router from 'vue-router';
import login from '@/components/login';
import board from '@/components/board';

Vue.use(Router);

const original = Router.prototype.push;

Router.prototype.push = function push(location) {
    return original.call(this, location).catch(err => err);
};

const router = new Router({
    routes: [
        {
            path: '/',
            redirect: '/login'
        },
        {
            path: '/login',
            name: 'login',
            component: login
        },
        {
            path: '/board',
            name: 'board',
            component: board
        }
    ]
});

router.beforeEach((to, from, next) => {
    if (to.path === '/login') {
        next();
    } else {
        let token = localStorage.getItem('Authorization');
        if (token === null || token === '') {
            next('/login');
        } else {
            next();
        }
    }
});

export default router;