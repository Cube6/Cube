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
        methods: {
            addDiscussionBoard(name) {
                this.$refs[name].validate((valid) => {
                    if (valid) {
                        this.axios({
                            method: 'post',
                            url: '/Board',
                            data: this.formInline,
                        }).then(() => {
                            this.$router.replace('/boardDetail');
                        }).catch(error => {
                            console.log(error);
                        })
                    } else {
                        this.$Message.error('Fail!');
                    }
                })
            }
        },
    }
</script>

<style>
</style>