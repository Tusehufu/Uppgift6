<template>
    <div class="modal show" style="display: block;" v-if="isVisible">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Bekräfta borttagning</h5>
                    <button type="button" class="btn-close" @click="closeModal"></button>
                </div>
                <div class="modal-body">
                    <p>Är du säker på att du vill radera detta svar?</p>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-danger" @click="confirmDelete">Radera</button>
                    <button class="btn btn-secondary" @click="closeModal">Avbryt</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, onMounted } from 'vue';
    import axios from 'axios';

    export default defineComponent({
        name: 'ConfirmDeleteReplyModal',
        props: {
            isVisible: {
                type: Boolean,
                required: true,
            },
            replyId: {
                type: Number,
                required: true,
            },
        },
        emits: ['confirm', 'cancel'],
        setup(props, { emit }) {
            onMounted(() => {
                console.log('Reply ID received in modal:', props.replyId);
            });

            // Funktion för att bekräfta borttagning av svar
            const confirmDelete = async () => {
                // Hämta JWT-token från din autentiseringskälla (t.ex. lokal lagring)
                const token = localStorage.getItem('jwtToken');
                console.log('Attempting to delete reply ID:', props.replyId); // Lägg till loggning
                // Skapa en HTTP-begäran med autentiseringsuppgifter (JWT-token)
                try {
                    const response = await axios.delete(`https://localhost:7056/api/Replies/${props.replyId}`, {
                        headers: {
                            Authorization: `Bearer ${token}`, // Skicka JWT-token som en del av Authorization-headers
                        },
                    });
                    emit('confirm', props.replyId);
                    window.location.reload();
                } catch (error) {
                    console.error("Det uppstod ett fel vid borttagning av svaret:", error);
                }
            };

            const closeModal = () => {
                emit('cancel');
            };

            return {
                confirmDelete,
                closeModal,
            };
        }
    });
</script>

<style scoped>
    /* Lägg till din CSS här om du vill */
</style>
