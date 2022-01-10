<template>
    <div style="margin:0">

        <!--<div style="text-align:right;margin:10px;">
            <Button type="primary" @click="AddBoard()">AddBoard</Button>
        </div>-->
        <ul>
            <li style="width:260px; float: left;">
                <Card style="width: 250px; cursor: pointer;">
                    <div style="display: flex; padding-bottom: 36px; background: #fff;">
                        <i scripturl="../scripts/font.js" style="font-size:24px;">
                            <svg viewBox="0 0 1024 1024" style="width:1em;height:1em;fill:currentColor;overflow:hidden">
                                <use xlink:href="#at-plus"></use>
                            </svg>
                        </i>
                    </div>
                    <span style="position:absolute;left:16px;bottom:12px;" @click="AddBoard()">Add Board</span>
                </Card>
            </li>
            <li v-for="board in post" :key="board.Id" style="width:260px; float: left;">
                <Card style="width: 250px; text-align: left; cursor: pointer">
                    <p slot="title">
                        <img :src="getUserAvatar(board.CreatedUser)" style="width:22px; height:22px; border-radius:50%; " />
                    </p>
                    <div style="text-align:center;">
                        <a href="#" slot="extra" @click.prevent="ViewBoard(board.Id)">
                            <Icon type="ios-loop-strong"></Icon>
                            {{board.Name}}
                        </a>
                    </div>
                    <a href="#" slot="extra" @click.prevent="DeleteBoard(board.Id)" title="Delete">
                        <span aria-label="Delete" class="">
                            <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(239, 83, 80);">
                                    <use xlink:href="#at-delete"></use>
                                </svg>
                            </button>
                        </span>
                    </a>
                </Card>
            </li>
        </ul>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        methods: {
            fetchData() {
                fetch('Board')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json;
                        return;
                    });
            },
            AddBoard() {
                this.$router.push('/addboard');
            },
            ViewBoard(val) {
                this.$router.push({ name:'boardDetail', params: { boardId: val } });
            },
            DeleteBoard(val) {
                this.axios.delete('/Board/' + val + '')
                    .then(() => {
                        this.fetchData();
                    })
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
</script>

<style>
    

</style>