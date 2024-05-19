<template>
    <div class="home">
        <h1 class="text-center" display="3">Forum</h1>
        <div class="container">
            <button class="btn btn-primary me-1" @click="openCreatePostModal">Skapa inlägg</button>
            <CreatePostModal :isVisible="isCreatePostModalVisible" @close="closeCreatePostModal" />
            <Posts :posts="posts" />

        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import Posts from '../../components/Posts.vue';
    import CreatePostModal from '../../components/CreatePostModal.vue';
    import axios from 'axios';

    interface Post {
        // Definiera strukturen för en post
    }

    export default defineComponent({
        name: 'Forumview',
        components: {
            Posts,
            CreatePostModal
            

        },
        data() {
            return {
                posts: [],
                isCreatePostModalVisible: false,
            };
        },
        mounted() {
            this.fetchPosts();
        },
        methods: {
            async fetchPosts() {
                try {
                    const response = await axios.get('https://localhost:7056/api/Post');
                    this.posts = response.data;
                } catch (error) {
                    console.error('Det uppstod ett fel vid hämtning av inlägg:', error);
                }
            },
            openCreatePostModal() {
                this.isCreatePostModalVisible = true;
            },
            closeCreatePostModal() {
                this.isCreatePostModalVisible = false;
            },

        }
    });
</script>

<style scoped>
    /* Stilregler för ForumView-komponenten */
</style>

