import ChangeHistory from '../components/About/ChangeHistory.vue';

export default {
    components: {
        ChangeHistory
    },
    data() {
        return {
            projects: [],
            activeProject: null,
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

        var isMenuCollapsed = localStorage.getItem('IsMenuCollapsed');
        if(isMenuCollapsed !=null)
        {
            this.isCollapsed = localStorage.getItem('IsMenuCollapsed') == "true";
        }
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
            localStorage.setItem('IsMenuCollapsed', this.isCollapsed );
        },
        fetchData(id) {
            this.fetchProjects();

            if (id == undefined || id == null) {
                    this.navURL = '/boardAll';
                    this.navName = null;
                    this.$router.replace(this.navURL).catch(error => {
                        if (
                          error.name !== 'NavigationDuplicated' &&
                          !error.message.includes('Avoided redundant navigation to current location')
                        ) {
                          console.log(error)
                        }});
            }
            else
            {
                this.navURL = '/boardAll/'+id;

                if (id == 0) {
                    this.navURL = '/boardAll';
                    this.navName = null;
                }
                else if(id == 1)
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
                sessionStorage.setItem('boardDetailPath', '')
                this.$router.replace(this.navURL);
            }
        },
        fetchProjects() {
            this.axios({
              method: 'get',
              url: '/Project'
            }).then((res) => {
              this.projects = res.data;

                var cachedProject = localStorage.getItem('ACTIVE_PROJECT_NAME');
                if(cachedProject){
                    this.activeProject = cachedProject;
                }else {
                    var firstProject = this.projects[0];
                    this.activeProject = firstProject.Name;
                    this.cacheProjectInfo(firstProject);
                }
            }).catch(error => {
              this.$Message.error('Failed to load projects. Error:' + error);
            });
        },
        setActiveProject(project){
            this.activeProject = project.Name;
            this.cacheProjectInfo(project);
            this.$router.replace('/boardAll');
        },
        cacheProjectInfo(project){
            localStorage.setItem('ACTIVE_PROJECT_NAME', project.Name);
            localStorage.setItem('ACTIVE_PROJECT_ID', project.Id);
        },
        goToUsers() {
            this.navURL = '/users';
            this.$router.replace(this.navURL);
        },
        logout() {
            //localStorage.setItem('LOGINUSER', '');
            localStorage.setItem('TOKEN', '');

            this.$router.push('/login');
            this.$router.go();
        },
        createAccount() {
            this.$router.push('/register');
        }
    },
}