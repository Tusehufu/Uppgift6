<template>
    <div class="modal show" style="display: block;" v-if="isVisible">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Svara på inlägg</h5>
                    <button type="button" class="btn-close" @click="closeModal"></button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent="submitForm">
                        <div class="mb-3">
                            <label for="replyContent" class="form-label">Svar</label>
                            <textarea id="replyContent" v-model="replyContent" class="form-control" required></textarea>
                        </div>
                        <button type="submit" class="btn btn-primary">Skicka svar</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref, watch } from 'vue';
    import axios from 'axios';

    interface ReplyData {
        content: string;
        postId: number; // Lägg till postId i interface
    }

    export default defineComponent({
        name: 'CreateReplyModal',
        props: {
            isVisible: {
                type: Boolean,
                required: true,
            },
            postId: {
                type: Number,
                required: true,
            },
        },
        setup(props, { emit }) {
            const replyContent = ref('');
            const postId = ref(props.postId); // Använd postId direkt från props
            console.log(postId);
            // Uppdatera postId när props.postId ändras
            watch(() => props.postId, (newPostId) => {
                postId.value = newPostId;
                console.log('postId updated:', postId.value);
            });

            const submitForm = async () => {
                /*try*/ {
                    console.log('postId when submitting form:', postId.value);
                    // Hämta JWT-token från din autentiseringskälla (t.ex. lokal lagring)
                    const token = localStorage.getItem('jwtToken');

                    // Skapa en HTTP-begäran med autentiseringsuppgifter (JWT-token)
                    const replyData: ReplyData = { content: replyContent.value, postId: postId.value }; // Inkludera postId
                    console.log('Data to be sent:', replyData);
                    const response = await axios.post<ReplyData>('https://localhost:7056/api/Replies', replyData, {
                        headers: {
                            Authorization: `Bearer ${token}`, // Skicka JWT-token som en del av Authorization-headers
                        },
                    });
                    console.log('Svar skapat:', response.data);
                    replyContent.value = '';
                    emit('close');
                } /*catch*/  {

                }
            };

            const closeModal = () => {
                emit('close');
            };

            return {
                replyContent,
                postId,
                submitForm,
                closeModal,
            };
        },
    });
</script>

<style scoped>
</style>
