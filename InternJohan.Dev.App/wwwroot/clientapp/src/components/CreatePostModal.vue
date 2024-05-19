<template>
    <div class="modal show" style="display: block;" v-if="isVisible">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Skapa nytt inlägg</h5>
                    <button type="button" class="btn-close" @click="closeModal"></button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent="submitForm">
                        <div class="mb-3">
                            <label for="title" class="form-label">Titel</label>
                            <input type="text" id="title" v-model="title" class="form-control" required />
                        </div>
                        <div class="mb-3">
                            <label for="content" class="form-label">Innehåll</label>
                            <textarea id="content" v-model="content" class="form-control" required></textarea>
                        </div>
                        <button type="submit" class="btn btn-primary">Spara</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref } from 'vue';
    import axios from 'axios';

    interface PostData {
        title: string;
        content: string;
        author: string;
    }

    export default defineComponent({
        name: 'CreatePostModal',
        props: {
            isVisible: {
                type: Boolean,
                required: true,
            },
        },
        setup(props, { emit }) {
            const title = ref('');
            const content = ref('');

            const submitForm = async () => {
                try {
                    // Hämta JWT-token från din autentiseringskälla (t.ex. lokal lagring)
                    const token = localStorage.getItem('jwtToken');

                    // Hämta användarinformationen från autentiseringskällan (t.ex. användarnamn eller ID)
                    const author = 'Användarnamn eller ID för inloggad användare';

                    // Skapa en HTTP-begäran med autentiseringsuppgifter (JWT-token) och författarinformation
                    const postData: PostData = { title: title.value, content: content.value, author: author };
                    const response = await axios.post<PostData>('https://localhost:7056/api/Post', postData, {
                        headers: {
                            Authorization: `Bearer ${token}`, // Skicka JWT-token som en del av Authorization-headers
                        },
                    });
                    console.log('Inlägg skapat:', response.data);
                    title.value = '';
                    content.value = '';
                    emit('close');
                } catch (error) {
                    console.error('Fel vid skapandet av inlägg:', error);
                }
            };


            const closeModal = () => {
                emit('close');
            };
            return {
                title,
                content,
                submitForm,
                closeModal,
            };
        },
    });
</script>

<style scoped>
    /* Din CSS-styling här */
</style>
