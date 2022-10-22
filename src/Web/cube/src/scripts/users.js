export default {
    data() {
        return {
            columns: [
                {
                    title: 'Name',
                    key: 'CreatedUser'
                },
                {
                    title: 'Items Submitted',
                    key: 'Count',
                    sortable: true
                },
            ],
            post: [
            ]
        };
    },
    created() {
        // fetch the data when the view is created and the data is
        // already being observed
        this.fetchData();
    },
    methods: {
        fetchData() {
            // fetch()
            //     .then(r => r.json())
            //     .then(json => {
            //         this.post = json;
            //         console.log(this.post);
            //         return;
            //     });


                this.axios(
                    {
                      method: 'get',
                      url: '/Statistics/boarditems'
                    })
                    .then(json => {
                      this.post = json.data;
                      //this.pageSetting.total = this.post.length;
                      console.log(this.post);
                      //setTimeout(msg);
                      return;
                    }).catch(error => {
                      //setTimeout(msg);
                      console.log('Failed to get statistics. Error:' + error);
                    })
        },
        ViewBoard(val) {
            this.$router.push({ name:'boardDetail', params: { boardId: val } });
        },
        getUserAvatar(userName) {
            let userAvatar = ""
            try {
              userAvatar = require('../assets/Person/' + userName.toLowerCase() + '.jpg')
              return userAvatar
            } catch (e) {
              userAvatar = require('../assets/Person/cube.jpg')
              return userAvatar
            }
          },
    },
}