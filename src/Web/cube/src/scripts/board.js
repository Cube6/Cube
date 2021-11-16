export default {
    name: "board",
    data() {
        return {
            loading: false,
            post: null
        };
    },
    created() {
        // fetch the data when the view is created and the data is
        // already being observed
        this.fetchData();
    },
    watch: {
        // call again the method if the route changes
        '$route': 'fetchData'
    },
    methods: {
        fetchData() {
            this.post = null;
            this.loading = true;

            fetch('User')
                .then(r => r.json())
                .then(json => {
                    this.post = json;
                    this.loading = false;
                    return;
                });
        }
    },
};