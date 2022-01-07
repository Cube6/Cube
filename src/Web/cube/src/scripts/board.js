export default {
    compontents: {
    },
    data() {
        return {
            isCollapsed: false,
        };
    },
    created() {
        // fetch the data when the view is created and the data is
        // already being observed
        this.fetchData();
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

        collapsedSider() {
            this.$refs.side1.toggleCollapse();
        },
        fetchData() {
            this.$router.push('/boardAll');
        },
        Logout() {
            this.$router.push('/login');
        }
    },
}