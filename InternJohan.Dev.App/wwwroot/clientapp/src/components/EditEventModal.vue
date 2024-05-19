<template>
    <div class="modal show" style="display: block;" v-if="isVisible">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Redigera evenemang</h5>
                    <button type="button" class="btn-close" @click="closeModal"></button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent="submitForm">
                        <div class="mb-3">
                            <label for="sport" class="form-label">Sport</label>
                            <input type="text" id="sport" v-model="localEditEvent.sport" class="form-control" required />
                        </div>
                        <div class="mb-3">
                            <label for="neededParticipants" class="form-label">Antal deltagare som behövs</label>
                            <input type="number" id="neededParticipants" v-model="localEditEvent.neededParticipants" class="form-control" required />
                        </div>
                        <div class="mb-3">
                            <label for="participants" class="form-label">Antal deltagare just nu</label>
                            <input type="number" id="participants" v-model="localEditEvent.participants" class="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="dateTime" class="form-label">Datum och tid</label>
                            <input type="datetime-local" id="dateTime" v-model="localEditEvent.dateTime" class="form-control" required />
                        </div>
                        <div class="mb-3">
                            <label for="location" class="form-label">Plats</label>
                            <input type="text" id="location" v-model="localEditEvent.location" class="form-control" required />
                        </div>
                        <button type="submit" class="btn btn-primary">Spara</button>
                    </form>
                    <div v-if="errorMessage" class="alert alert-danger mt-3">
                        {{ errorMessage }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref, watch } from 'vue';
    import axios from 'axios';

    export default defineComponent({
        name: 'EditEventModal',
        props: {
            isVisible: {
                type: Boolean,
                required: true,
            },
            editEvent: {
                type: Object,
                required: true,
            },
        },
        emits: ['close', 'save'],
        setup(props, { emit }) {
            const errorMessage = ref<string | null>(null);
            const localEditEvent = ref<any>({ ...props.editEvent });

            watch(() => props.editEvent, (newEditEvent) => {
                localEditEvent.value = { ...newEditEvent };
            });

            const submitForm = async () => {
                const userId = localStorage.getItem('userId');
                if (!userId) {
                    errorMessage.value = 'Användar-ID saknas.';
                    return;
                }
                console.log(localEditEvent.value.id);
                // Hämta JWT-token från din autentiseringskälla (t.ex. lokal lagring)
                const token = localStorage.getItem('jwtToken');
                // Skapa en HTTP-begäran med autentiseringsuppgifter (JWT-token)
                try {
                    const { userHost, ...eventData } = localEditEvent.value;
                    const response = await axios.put(`https://localhost:7056/api/SportEvent/${localEditEvent.value.id}`, eventData, {
                        headers: {
                            Authorization: `Bearer ${token}`, // Skicka JWT-token som en del av Authorization-headers
                        },
                    });

                    emit('save');
                } catch (error) {
                    console.error('Error saving event:', error);
                    errorMessage.value = 'Något gick fel vid uppdateringen av sportevenemanget.';
                }
            };

            const closeModal = () => {
                emit('close');
            };

            return {
                submitForm,
                closeModal,
                errorMessage,
                localEditEvent,
            };
        },
    });
</script>

<style scoped>
    /* Lägg till din CSS här om du vill */
</style>
