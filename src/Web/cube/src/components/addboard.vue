<template>
    <div class="subLayout">
        <Form ref="formInline" label-position="right" :model="formInline" :rules="ruleInline">
            <FormItem prop="name" class="lgD">
                <Input v-model="formInline.name" spellcheck="true" clearable 
                        placeholder="Enter a board name. Eg: Runtime/DDW SprintX Retro"/>
            </FormItem>
            <FormItem>
                <Button @click="addDiscussionBoard('formInline')" type="primary" >Add</Button>
                <Button @click="cancel()" style="margin-left: 8px" >Cancel</Button>
            </FormItem>
        </Form>
    </div>
</template>

<script>
	import addboard from "../scripts/addboard.js";
	export default addboard;
</script>

<style>
	@import "../styles/board.css";
</style>