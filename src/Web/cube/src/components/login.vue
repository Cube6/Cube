<template>
    <div class="wrap" id="wrap">
        <div class="logGet">
            <div class="logD logDtip">
                <p class="p1">Cube Retro System</p>
            </div>
            <Form ref="formInline" label-position="right" :model="formInline" :rules="ruleInline">
                <FormItem prop="user" class="lgD">
                    <Input type="text" v-model="formInline.user" placeholder="Username">
                    <Icon type="ios-person-outline" slot="prepend"></Icon>
                    </Input>
                </FormItem>
                <FormItem prop="password" class="lgD">
                    <Input type="password" v-model="formInline.password" password placeholder="Password">
                    <Icon type="ios-lock-outline" slot="prepend" style="z-index:1"></Icon>
                    </Input>
                </FormItem>
                <FormItem>
                    <Button type="primary" @click="handleSubmit('formInline')">Login</Button>
                </FormItem>
            </Form>
        </div>
    </div>

</template>

<script>
    import login from "../scripts/login.js";
    export default login;
</script>

<style>
    @import "../styles/login.css";
</style>