<template>
    <div class="subLayout">
        <Form ref="formInline" label-position="right" :model="formInline" :rules="ruleInline">
            <FormItem prop="name" class="lgD">
                <Input v-model="formInline.name" spellcheck="true"
                        placeholder="Enter a board name. Eg: Runtime/DDW SprintX Retro"/>
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
                        { required: true, message: 'Board Name is required', trigger: 'blur'},
                        { max: 100, message: 'The length of Board Name should be less than 100', trigger: 'blur'}
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
                            data: this.formInline
                        }).then(res => {
                            this.$router.push({ name: 'boardDetail', params: { boardId: res.data, boardName: this.formInline.name, createdUser:this.formInline.CreatedUser, state:1 } });
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
    .subLayout{
        margin: 0;
        padding: 12px;
    }

    .subLayout .ivu-card-body {
        padding: 16px;
    }
</style>