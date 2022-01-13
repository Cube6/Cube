<template>
    <div id="app">
        <router-view></router-view>
        <vue-particles id="particles-js"> </vue-particles>
    </div>
</template>

<script>
    
export default {
}
</script>

<style>
    body {
        background-image: url(./assets/login_bg.png);
        background-size: cover;
        height: calc(100% - 10px);
        font-family: sans-serif;
        margin: 0;
        padding: 0;
    }

    #particles-js {
        width: 100%;
        height: calc(100% - 10px);
        position: absolute;
        z-index: -9999;
    }

</style>
