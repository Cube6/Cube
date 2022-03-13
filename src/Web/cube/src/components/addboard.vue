<template>
    <div>
        <Form ref="formInline" label-position="right" :model="formInline" :rules="ruleInline">
            <FormItem prop="name" class="lgD">
                <Input v-model="formInline.name" placeholder="Boardname"/>
            </FormItem>
            <FormItem>
                <Button @click="addDiscussionBoard('formInline')" type="primary" >Add</Button>
                <Button @click="cancel()" style="margin-left: 8px" >Cancel</Button>
            </FormItem>
        </Form>
    </div>
</template>

<script>
    const signalR = require("@microsoft/signalr");
    export default {
        data() {
            return {
                UserToken: null,
                formInline: {
                    Name: "",
                    CreatedUser: "",
                },
                ruleInline: {
                    name: [
                        { required: true, message: 'Please fill in the board name', trigger: 'blur' }
                    ]
                },

                connection: "",
            };
        },
        created() {
            this.UserToken = localStorage.getItem('TOKEN');
            this.formInline.CreatedUser = localStorage.getItem('LOGINUSER');
            this.init();
        },
        methods: {
            addDiscussionBoard(name) {
                this.$refs[name].validate((valid) => {
                    if (valid) {
                        this.axios({
                            method: 'post',
                            url: '/Board',
                            data: this.formInline,
                            headers: {
                                'Authorization': 'Bearer ' + this.UserToken
                            }
                        }).then(res => {
                            this.$router.push({ name: 'boardDetail', params: { boardId: res.data, boardName: this.formInline.name, state:1 } });
                        }).then(() => {
                            this.sendBoardMessage();
                        }).then(() => {
                            this.renderFunc(this.formInline.name + ' is created successfully.');
                        }).catch(error => {
                            console.log('Failed to addDiscussionBoard, Error :' + error);
                        })
                    } else {
                        this.$Message.error('Fail!');
                    }
                })
            },
            cancel() {
                this.$router.push('/boardAll');
            },
            renderFunc(message) {
                this.$Notice.success({
                    desc: 'The desc will hide when you set render.',
                    render: h => {
                        return h('span', [
                            message
                        ])
                    }
                });
            },
            init() {
                this.connection = new signalR.HubConnectionBuilder()
                    .withUrl("http://10.63.224.86:5050/BoardHub", {})
                    .configureLogging(signalR.LogLevel.Error)
                    .build();
                this.connection.start();
            },
            sendBoardMessage() {
                this.connection.invoke("SendBoardMessage");
            }
        },
    }
</script>

<style>
</style>