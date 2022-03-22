export default {
    compontents: {
    },
    data() {
        return {
            isCollapsed: false,
            userName: null,
            navURL:null,
            navName:null,

            showMyProfile: false,
            showAboutView: false,
            pStyle: {
                fontSize: '16px',
                color: 'rgba(0,0,0,0.85)',
                lineHeight: '24px',
                display: 'block',
                marginBottom: '16px'
            }
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
        getLogo() {
            return require('../assets/logo.jpg');
        },
        collapsedSider() {
            this.$refs.side1.toggleCollapse();
        },
        fetchData(id) {
            if(id == null)
            {
                this.navURL = '/boardAll';
                this.navName = null;
            }
            else
            {
                this.navURL = '/boardAll/'+id;
               
                if(id==1)
                {
                    this.navName = "In Progress";
                }
                else if(id == 2)
                {
                    this.navName = "Completed";
                }
                else if(id == 3)
                {
                    this.navName = "Recycle Bin";
                }
            }

            this.$router.push(this.navURL);
        },
        logout() {

            localStorage.setItem('LOGINUSER', '');
            localStorage.setItem('TOKEN', '');

            this.$router.push('/login');
        },
        createAccount() {
            this.$router.push('/register');
        }
    },
}