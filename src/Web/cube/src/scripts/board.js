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
        }
    },
    methods: {
        fetchData() {
            this.$router.push('/boardAll');
        },
        AddBoard() {
            this.$router.push('/addboard');
        },
    },
}