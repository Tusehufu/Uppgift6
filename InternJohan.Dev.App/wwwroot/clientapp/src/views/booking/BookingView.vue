<template>
    <div class="booking">
        <h1 class="text-center">Välkommen till bokning</h1>
        <div class="container">
            <div class="row">
                <div class="embed-responsive embed-responsive-16by9">
                    <iframe src="https://gislaved.actorsmartbook.se/ResourceBookingRequest.aspx" width="900" height="600"></iframe>
                </div>
            </div>
        </div>
    </div>
    <p class="text-center">Gå gärna in och boka via <a href="https://gislaved.actorsmartbook.se/ResourceBookingRequest.aspx">Kommunens hemsida</a></p>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        name: 'BookingView',
        components: {
        },
    });
</script>
