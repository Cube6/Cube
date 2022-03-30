import Vue from 'vue'
import Router from 'vue-router';
import login from '@/components/login';
import register from '@/components/register';
import board from '@/components/board';
import addboard from '@/components/addboard';
import boardAll from '@/components/boardAll';
import boardDetail from '@/components/boardDetail';

Vue.use(Router);

const original = Router.prototype.push;

Router.prototype.push = function push(location) {
    return original.call(this, location).catch(err => err);
};

const router = new Router({
    mode: 'history',
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
            path: '/register',
            name: 'register',
            component: register
        },
        {
            path: '/board',
            name: 'board',
            component: board,
            children: [//二级路由
                {
                    path: '/addboard',
                    name: 'addboard',
                    component: addboard
                },
                {
                    path: '/boardAll/:type?',
                    name: 'boardAll',
                    component: boardAll
                },
                {
                    path: '/boardDetail',
                    name: 'boardDetail',
                    component: boardDetail
                }
            ]
        }
    ]
});

//router.beforeEach((to, from, next) => {
//    if (to.path === '/login') {
//        next();
//    } else {
//        let token = localStorage.getItem('Authorization');
//        if (token === null || token === '') {
//            next('/login');
//        } else {
//            next();
//        }
//    }
//});

export default router;