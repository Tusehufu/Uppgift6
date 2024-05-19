<template>
    <div class="row">
        <div class="col-6">
        </div>
        <div v-for="reply in replies" :key="reply.replyId" class="reply">
            <div class="col-6">
                <div class="border">
                    <p>{{ reply.content }}</p>
                    <p>{{ reply.timestamp }}</p>
                    <p>{{ reply.user_id }}</p>
                    <p>Användare: {{ reply.author }}</p>
                </div>
                <button class="btn btn-danger" @click="showConfirmDeleteModal(reply.replyId)">Radera svar</button>
                <ConfirmDeleteReplyModal :isVisible="isConfirmDeleteModalVisible" :replyId="selectedReplyIdToDelete" @confirm="deleteReply" @cancel="closeConfirmDeleteModal" />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, PropType, ref } from 'vue';
    import ConfirmDeleteReplyModal from './ConfirmDeleteReplyModal.vue';

    interface Reply {
        replyId: number;
        content: string;
        user_id: number;
        timestamp: string;
        author: string;
    }

    export default defineComponent({
        name: 'Replies',
        components: {
            ConfirmDeleteReplyModal,
        },
        props: {
            replies: {
                type: Array as PropType<Reply[]>,
                required: true
            }
        },
        setup() {
            const isConfirmDeleteModalVisible = ref(false);
            const selectedReplyIdToDelete = ref<number | null>(null);

            const showConfirmDeleteModal = (replyId: number) => {
                console.log('Opening delete modal for reply ID:', replyId); 
                selectedReplyIdToDelete.value = replyId;
                isConfirmDeleteModalVisible.value = true;
            };

            const closeConfirmDeleteModal = () => {
                selectedReplyIdToDelete.value = null;
                isConfirmDeleteModalVisible.value = false;
            };

            const deleteReply = (replyId: number) => {
                console.log('Deleting reply ID:', replyId); 
                closeConfirmDeleteModal();
            }

            return {
                isConfirmDeleteModalVisible,
                showConfirmDeleteModal,
                closeConfirmDeleteModal,
                deleteReply,
                selectedReplyIdToDelete,
            };
        }
    });
</script>

<style scoped>
</style>
