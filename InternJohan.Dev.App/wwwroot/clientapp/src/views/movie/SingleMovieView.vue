<!--<template>
    <div>
        <h1>Filmen du sökte</h1>
            <div class="mb-3">
                <label for="id" class="form-label">Movie Id</label>
                <input v-model="movieId" type="text" class="form-control" id="id">
            </div>
            <button type="button" class="btn btn-primary" @click="search">Submit</button>

        <div v-if="selectedMovie">
            <h2>Vald film:</h2>
            <p>{{ selectedMovie.title }}</p>
            <p>{{ selectedMovie.displayRealeaseYear }}</p>
            <p>{{ selectedMovie.averageRating }}</p>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { onMounted, ref } from 'vue';
    import axios from 'axios';
    //import { useRoute } from 'vue-router'

    //const route = useRoute()

    const movieId = ref(null);
    const selectedMovie = ref({
        title: '',
        displayRealeaseYear: '',
        averageRating: ''
    });

    const search = async () => {
        if (movieId.value !== null && movieId.value > 0) {
            const response = await axios.get('https://localhost:7056/api/movie/' + movieId.value)

            selectedMovie.value.title = response.data.title;
            selectedMovie.value.displayRealeaseYear = response.data.displayRealeaseYear;
            selectedMovie.value.averageRating = response.data.averageRating;

            console.log(response.data);
        }
    };

    // Ladda filmer när komponenten har monterats
    //onMounted(async () => {
    //    if (route.params.id) {
    //        movieId.value = parseInt(route.params.id);
    //        await search();
    //    }
    //});
</script>-->
