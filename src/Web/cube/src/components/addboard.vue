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
    import axios from 'axios';
    export default {
        data() {
            return {
                formInline: {
                    Name: "",
                    DateCreated: new Date("2021-11-19 10:23:22")
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
                        console.log(this.formInline);
                        axios({
                            method: 'post',
                            url: '/Board',
                            data: this.formInline,
                        }).then(() => {
                            this.$Message.success('Success!');
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