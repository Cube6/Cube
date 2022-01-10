export default {
    compontents: {
    },
    data() {
        return {
            isCollapsed: false,
            userName: null,
        };
    },
    created() {
        // fetch the data when the view is created and the data is
        // already being observed
        this.fetchData();
        this.userName = localStorage.getItem('LOGINUSER').toUpperCase();
    },
    computed: {
        menuitemClasses: function () {
            return [
                'menu-item',
                this.isCollapsed ? 'collapsed-menu' : ''
            ]
        },
        rotateIcon() {
            return [
                'menu-icon',
                this.isCollapsed ? 'rotate-icon' : ''
            ];
        },
    },
    methods: {
        getLoginUserAvatar() {
            let userAvatar = ""
            try {
                userAvatar = require('../assets/Person/' + this.userName.toLowerCase() + '.jpg')
                return userAvatar
            } catch (e) {
                userAvatar = require('../assets/Person/cube.jpg')
                return userAvatar
            }
        },
        collapsedSider() {
            this.$refs.side1.toggleCollapse();
        },
        fetchData() {
            this.$router.push('/boardAll');
        },
        Logout() {

            localStorage.setItem('LOGINUSER', null);
            localStorage.setItem('TOKEN', null);

            this.$router.push('/login');
        }
    },
}