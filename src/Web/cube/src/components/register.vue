﻿<template>
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
            <h2>Create your account</h2>
            <Form ref="formInline" label-position="right" :model="formInline" :rules="ruleInline">
                <div prop="user" class="inputBox">
                    <input type="text" v-model="formInline.user" required="">
                    <label>Username</label>
                </div>
                <div prop="password" class="inputBox">
                    <input type="password" v-model="formInline.password" password required="">
                    <label>Password</label>
                </div>
                <div prop="confirmPassword" class="inputBox">
                    <input type="password" v-model="formInline.confirmPassword" password required="">
                    <label>Confirm Password</label>
                </div>
                <Button type="success" size="large" long :loading="loading" @click="register('formInline')">
                    <span v-if="!loading">Sign up</span>
                    <span v-else>Sign up...</span>
                </Button>
            </Form>

            <p class="login-callout mt-3">
                Already have an account?
                <a @click="redirectToLogin()" href="#">Sign in</a>
            </p>
            <p class="login-tip">
                &copy; 2023 Cube6
            </p>
        </div>
    </div>
</template>

<script>
    import register from "../scripts/register.js";
    export default register;
</script>

<style>
    @import "../styles/login.css";
</style>