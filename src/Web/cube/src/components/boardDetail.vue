<template>
    <div style="height: 600px">
        <table>
            <thead>
                <tr>
                    <th width="400px">What went well</th>
                    <th width="400px">What need to be imporved</th>
                    <th width="200px">Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <ul>
                            <li v-for="well in post" :key="well.Id">
                                {{well.Name}}
                            </li>
                        </ul>
                        <textarea></textarea>
                        <Button type="primary" @click="AddWentWell()">Send</Button>
                    </td>
                    <td>
                        <ul>
                            <li v-for="imporve in post" :key="imporve.Id">
                                {{imporve.Name}}
                            </li>
                        </ul>
                        <textarea></textarea>
                        <Button type="primary" @click="AddImporved()">Send</Button>
                    </td>
                    <td>
                        <ul>
                            <li v-for="action in post" :key="action.Id">
                                {{action.Name}}
                            </li>
                        </ul>
                        <textarea></textarea>
                        <Button type="primary" @click="AddAction()">Send</Button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        methods: {
            fetchData() {
                fetch('Board')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json;
                        return;
                    });
            },
            AddWentWell() { },
            AddImporved() { },
            AddAction()
        },
    }
</script>

<style>
    ul {
        list-style-type: none;
    }
</style>