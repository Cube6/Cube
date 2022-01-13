<template>
    <div id="loginArea">
        <h2>Login</h2>
        <Form ref="formInline" label-position="right" :model="formInline" :rules="ruleInline">
            <div prop="user" class="inputBox">
                <input type="text" v-model="formInline.user" required="">
                <label>Username</label>
            </div>
            <div prop="password" class="inputBox">
                <input type="password" v-model="formInline.password" password required="">
                <label>Password</label>
            </div>
            <input type="submit" value="Login" @click="handleSubmit('formInline')">
        </Form>
        <p class="login-tip">Welcome to Cube System v0.1</p>
    </div>
</template>

<script>
    import login from "../scripts/login.js";
    export default login;
</script>

<style>
    @import "../styles/login.css";
</style>