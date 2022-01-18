import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

const store = new Vuex.Store({

    state: {
        //store token
        Authorization: localStorage.getItem('TOKEN') ? localStorage.getItem('TOKEN') : ''
    },

    mutations: {
        // update token, and store token to localStorage

        changeLogin(state, user) {
            state.Authorization = user.Authorization;
            localStorage.setItem('TOKEN', user.Authorization);
        }
    }

});


export default store;

