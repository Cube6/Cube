<template>
    <div>
        <Form ref="formInline" label-position="right" :model="formInline" :rules="ruleInline">
            <FormItem prop="name" class="lgD">
                <Input v-model="formInline.name" placeholder="Boardname">
                </Input>
            </FormItem>
            <FormItem>
                <Button  @click="addDiscussionBoard('formInline')">Add</Button>
            </FormItem>
        </Form>
    </div>
</template>

<script>
   
    export default {
        data() {
            return {
                UserToken:null,
                formInline: {
                    Name: "",
                    CreatedUser:"Michael",
                },
                ruleInline: {
                    name: [
                        { required: true, message: 'Please fill in the board name', trigger: 'blur' }
                    ]
                }
            };
        },
        created() {
            this.UserToken = localStorage.getItem('TOKEN');
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
                        }).then(() => {
                            this.renderFunc(this.formInline.name + ' is created successfully.');
                        }).then(() => {
                            this.$router.replace('/boardDetail');
                        }).catch(error => {
                            console.log(error);
                        })
                    } else {
                        this.$Message.error('Fail!');
                    }
                })
            },
            renderFunc(message) {
                this.$Notice.success({
                    title: 'Notification',
                    desc: 'The desc will hide when you set render.',
                    render: h => {

                        return h('span', [
                            message
                        ])

                        //return h('span', [
                        //    'This is created by ',
                        //    h('a', 'render'),
                        //    ' function'
                        //])
                    }
                });
            }
        },
    }
</script>

<style>
</style>