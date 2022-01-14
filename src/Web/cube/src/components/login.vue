<template>

    <div class="loginAll">
        <vue-particles class="login-bg"
                       color="#39AFFD"
                       :particleOpacity="0.7"
                       :particlesNumber="100"
                       shapeType="circle"
                       :particleSize="4"
                       linesColor="#8DD1FE"
                       :linesWidth="1"
                       :lineLinked="true"
                       :lineOpacity="0.4"
                       :linesDistance="150"
                       :moveSpeed="3"
                       :hoverEffect="true"
                       hoverMode="grab"
                       :clickEffect="true"
                       clickMode="push">
        </vue-particles>
        <div id="loginArea">
            <h2>Sign in to Cube</h2>
            <Form ref="formInline" label-position="right" :model="formInline" :rules="ruleInline">
                <div prop="user" class="inputBox">
                    <input type="text" v-model="formInline.user" required="">
                    <label>Username</label>
                </div>
                <div prop="password" class="inputBox">
                    <input type="password" v-model="formInline.password" password required="">
                    <label>Password</label>
                    <a>Forgot Password?</a>
                </div>
                <input type="submit" value="Sign in" @click="handleSubmit('formInline')">
            </Form>

            <p class="login-callout mt-3">
                New to Cube?
                <a>Create an account.</a>
            </p>
            <p class="login-tip">Welcome to Cube System v0.1</p>
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