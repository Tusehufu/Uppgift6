<template>
    <div class="booking">
        <h1 class="text-center">Välkommen till alla idrottsevenemang</h1>
        <h2 class="text-center">Här kan du se alla idrottsevenemang och gå med i idrottsevenemanget</h2>
        <div class="container">
            <div class="row">
                <div v-for="sportevent in sportevents" :key="sportevent.id" class="col">
                    <div class="card mt-3" style="width: 18rem;">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item">Sport: {{ sportevent.sport }}</li>
                            <li class="list-group-item">Antal deltagare: {{ sportevent.participants }}</li>
                            <li class="list-group-item">Antal deltagare som behövs: {{ sportevent.neededParticipants }}</li>
                            <li class="list-group-item">Tid: {{ sportevent.dateTime }}</li>
                            <li class="list-group-item">Plats: {{ sportevent.location }}</li>
                            <li class="list-group-item">
                                Deltagare
                                <ul>
                                    <li v-for="attendee in sportevent.attendees" :key="attendee.userId">{{ attendee.username }}</li>
                                </ul>
                            </li>
                            <li class="list-group-item d-none">ID: {{ sportevent.id }}</li>
                        </ul>
                        <div class="card-footer">
                            <button v-if="!sportevent.isJoined" class="btn btn-success me-1" @click="joinEvent(sportevent.id)" :disabled="sportevent.isJoined">Gå med</button>
                            <button v-else class="btn btn-danger me-1" @click="leaveEvent(sportevent.id)">Gå ur</button>
                            <button class="btn btn-danger me-1" @click="openConfirmDeleteModal(sportevent.id)">Radera</button>
                            <button class="btn btn-primary" @click="openEditModal(sportevent)">Redigera</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <EditEventModal :isVisible="isModalVisible"
                    :editEvent="editEvent"
                    @close="closeModal"
                    @eventUpdated="handleEventUpdated" />
    <ConfirmDeleteModal :isVisible="isConfirmDeleteVisible"
                        :eventId="eventIdToDelete"
                        @confirm="deleteSportEvent"
                        @cancel="closeConfirmDeleteModal" />
</template>

<script lang="ts">
    import { defineComponent, ref, onMounted } from 'vue';
    import axios from 'axios';
    import EditEventModal from '../../components/EditEventModal.vue';
    import ConfirmDeleteModal from '../../components/ConfirmDeleteModal.vue';

    interface Attendee {
        userId: number;
        eventId: number;
        userName: string;
    }

    interface SportEvent {
        id: number;
        sport: string;
        participants: number;
        neededParticipants: number;
        dateTime: string;
        location: string;
        isJoined: boolean;
        attendees: Attendee[]; 
    }

    export default defineComponent({
        name: 'SportsView',
        components: {
            EditEventModal,
            ConfirmDeleteModal,
        },
        setup() {
            const sportevents = ref<SportEvent[]>([]);
            const isModalVisible = ref(false);
            const editEvent = ref<any>({});
            const isConfirmDeleteVisible = ref(false);
            const eventIdToDelete = ref<number | null>(null);

            const getAttendees = async (eventId: number): Promise<Attendee[]> => {
                const response = await axios.get(`https://localhost:7056/api/Attendee/${eventId}`);
                return response.data;
            };

            const getSportEvents = async () => {
                try {
                    const response = await axios.get('https://localhost:7056/api/SportEvent');
                    const events = response.data;
                    const userId = localStorage.getItem('userId');
                    for (let event of events) {
                        event.attendees = await getAttendees(event.id);
                        for (const attendee of event.attendees) {
                            if (attendee.userId == userId) {
                                event.isJoined = true;
                            } 
                            
                        }
                    }
                    console.log(events);
                    sportevents.value = events;
                } catch (error) {
                    console.error('Fel vid hämtning av sportevenemang:', error);
                }
            };

            const joinEvent = async (eventId: number) => {
                try {
                    const token = localStorage.getItem('jwtToken');
                    const response = await axios.post(`https://localhost:7056/api/SportEvent/${eventId}/join`, {}, {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    });
                    if (response.data.success) {
                        const eventIndex = sportevents.value.findIndex((event: any) => event.id === eventId);
                        if (eventIndex !== -1) {
                            sportevents.value[eventIndex].participants++;
                            sportevents.value[eventIndex].isJoined = true;
                            sportevents.value[eventIndex].attendees = await getAttendees(eventId); // Uppdatera deltagarlistan
                        }
                    } else {
                        console.error('Failed to join the event:', response.data.error);
                    }
                } catch (error) {
                    console.error('Fel vid anslutning till evenemanget:', error);
                }
            };

            const leaveEvent = async (eventId: number) => {
                const token = localStorage.getItem('jwtToken');
                const userId = localStorage.getItem('userId');
                if (!token || !userId) {
                    throw new Error('Token or userId not found');
                }
                const response = await axios.delete(`https://localhost:7056/api/SportEvent/${eventId}/participant/${userId}`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                    data: {
                        userId: Number(userId),
                    },
                });
                if (response.data.success) {
                    const eventIndex = sportevents.value.findIndex((event: any) => event.id === eventId);
                    if (eventIndex !== -1) {
                        sportevents.value[eventIndex].participants--;
                        sportevents.value[eventIndex].isJoined = false;
                        // Ta bort den aktuella användaren från deltagarlistan
                        
                        sportevents.value[eventIndex].attendees = await getAttendees(eventId); // Uppdatera deltagarlistan

                    }
                } else {
                    console.error('Failed to leave the event:', response.data.error);
                }
            }
            const openEditModal = (sportevent: any) => {
                editEvent.value = sportevent;
                isModalVisible.value = true;
            };

            const closeModal = () => {
                isModalVisible.value = false;
            };

            const handleEventUpdated = () => {
                getSportEvents();
                isModalVisible.value = false;
            };

            const openConfirmDeleteModal = (eventId: number) => {
                eventIdToDelete.value = eventId;
                isConfirmDeleteVisible.value = true;
            };

            const closeConfirmDeleteModal = () => {
                isConfirmDeleteVisible.value = false;
            };

            const deleteSportEvent = async (eventId: number) => {
                closeConfirmDeleteModal();
            };

            onMounted(() => {
                getSportEvents();
            });

            return {
                sportevents,
                isModalVisible,
                editEvent,
                isConfirmDeleteVisible,
                eventIdToDelete,
                joinEvent,
                leaveEvent,
                openEditModal,
                closeModal,
                handleEventUpdated,
                openConfirmDeleteModal,
                closeConfirmDeleteModal,
                deleteSportEvent,
            };
        },
    });
</script>
