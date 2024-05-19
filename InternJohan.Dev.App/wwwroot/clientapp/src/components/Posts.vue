<template>
    <div class="row">
        <div v-for="post in posts" :key="post.postId" class="post">
            <div class="border">
                <h3>{{ post.title }}</h3>
                <p>{{ post.content }}</p>
                <p>{{ post.timestamp }}</p>
                <p>Skrivet av: {{ post.author }}</p>
            </div>
            <button class="btn btn-primary" @click="openCreateReplyModal(post.postId)">Svara</button>
            <CreateReplyModal :isVisible="isCreateReplyModalVisible" @close="closeCreateReplyModal" />
            <button class="btn btn-danger" @click="showConfirmDeleteModal(post.postId)">Radera</button>
            <ConfirmDeletePostModal :isVisible="isConfirmDeleteModalVisible" :postId="selectedPostIdToDelete" @confirm="deletePost" @cancel="closeConfirmDeleteModal" />
            <Replies :replies="post.replies" />
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, PropType, ref, onMounted, watch } from 'vue';
    import CreateReplyModal from './CreateReplyModal.vue';
    import ConfirmDeletePostModal from './ConfirmDeletePostModal.vue';
    import Replies from './Replies.vue';
    import axios from 'axios';

    interface Post {
        postId: number;
        title: string;
        content: string;
        timestamp: string;
        author: string;
        replies?: Reply[];
    }

    interface Reply {
        replyId: number;
        content: string;
        author: string;
        timestamp: string;
        postId: number;
    }

    export default defineComponent({
        name: 'Posts',
        components: {
            CreateReplyModal,
            ConfirmDeletePostModal,
            Replies
        },
        props: {
            posts: {
                type: Array as PropType<Post[]>,
                required: true
            }
        },
        setup(props) {
            const isCreateReplyModalVisible = ref(false);
            const isConfirmDeleteModalVisible = ref(false);
            const selectedPostIdToDelete = ref<number | null>(null);
            const selectedPostId = ref<number | null>(null);

            const openCreateReplyModal = (postId: number) => {
                selectedPostId.value = postId; // Sätt selectedPostId till postId för den valda posten
                console.log(selectedPostId);
                isCreateReplyModalVisible.value = true;
            };

            const closeCreateReplyModal = () => {
                isCreateReplyModalVisible.value = false;
            };

            const showConfirmDeleteModal = (postId: number) => {
                selectedPostIdToDelete.value = postId;
                isConfirmDeleteModalVisible.value = true;
            };

            const closeConfirmDeleteModal = () => {
                selectedPostIdToDelete.value = null;
                isConfirmDeleteModalVisible.value = false;
            };

            const deletePost = (postId: number) => {
                closeConfirmDeleteModal();
            };

            const fetchRepliesForPost = async (postId: number) => {
                try {
                    const response = await axios.get<Reply[]>(`https://localhost:7056/api/postreplies/${postId}/replies`);
                    return response.data;
                } catch (error) {
                    console.error('Det uppstod ett fel vid hämtning av svar:', error);
                    return [];
                }
            };

            const fetchAllReplies = async () => {
                const updatedPosts = await Promise.all(
                    props.posts.map(async post => {
                        const replies = await fetchRepliesForPost(post.postId);
                        return { ...post, replies };
                    })
                );
                posts.value = updatedPosts;
            };

            const posts = ref<Post[]>(props.posts);

            onMounted(async () => {
                await fetchAllReplies();
            });

            watch(() => props.posts, async (newPosts) => {
                posts.value = newPosts;
                await fetchAllReplies();
            });

            return {
                isCreateReplyModalVisible,
                isConfirmDeleteModalVisible,
                openCreateReplyModal,
                closeCreateReplyModal,
                showConfirmDeleteModal,
                closeConfirmDeleteModal,
                deletePost,
                selectedPostIdToDelete,
                posts
            };
        }
    });
</script>

<style scoped>
    /* Lägg till din CSS här om du vill */
</style>