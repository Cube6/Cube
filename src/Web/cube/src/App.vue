<template>
    <div id="app">
        <router-view></router-view>
        <vue-confirm-dialog></vue-confirm-dialog>
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

</style>
