export default {
    data() {
        return {
            columns: [
                {
                    title: 'Name',
                    slot: 'name'
                },
                {
                    title: 'Items Submitted',
                    key: 'Count',
                    sortable: true
                },
                {
                    title: 'Action',
                    slot: 'action',
                    width: 200,
                    align: 'center'
                }     
            ],
            data: [
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
            this.axios(
                {
                  method: 'get',
                  url: '/Statistics/boarditems'
                })
                .then(json => {
                  this.data = json.data;
                  console.log(this.data);
                  return;
                }).catch(error => {
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
        show (index) {
          this.$Modal.info({
              title: 'User Info',
              content: `Name：${this.data[index].CreatedUser}<br>Items Submitted: ${this.data[index].Count}`
          })
        },
        remove (index) {
            this.data.splice(index, 1);
        }
    },
}