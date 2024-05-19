<!--<template>
    <div>
        <h1>Filmen du vill uppdatera</h1>
        <form @submit.prevent="onSubmit">-->
            <!--<div class="mb-3">
                <label for="id" class="form-label">Movie Id</label>
                <input v-model="movieId" type="text" class="form-control" id="id">
            </div>-->
            <!--<div class="mb-3">
                <label for="title" class="form-label">Movie Title</label>
                <input v-model="title" type="text" class="form-control" id="title">
            </div>
            <div class="mb-3">
                <label for="releaseYear" class="form-label">Release year</label>
                <input v-model="releaseYear" type="text" class="form-control" id="releaseYear">
            </div>
            <div class="mb-3">
                <label for="averageRating" class="form-label">average rating</label>
                <input v-model="averageRating" type="text" class="form-control" id="averageRating">
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
        </form>
    </div>
</template>

<script setup lang="ts">
    import { onMounted, ref } from 'vue';
    import axios from 'axios';
    import { useRoute } from 'vue-router'

    const route = useRoute()
    const title = ref('');
    const releaseYear = ref('');
    const averageRating = ref('');

    const getMovies = async () => {
        try {
            const response = await axios.get('https://localhost:7056/api/movie/' + route.params.id);
            title.value = response.data.title
            releaseYear.value = response.data.releaseYear
            averageRating.value = response.data.averageRating
        } catch (error) {
            console.error('Error fetching movies:', error);
        }
    };

    const onSubmit = async () => {
        const data = {
            title: title.value,
            releaseYear: releaseYear.value,
            averageRating: averageRating.value
        };
        const response = await axios.put('https://localhost:7056/api/movie/' + route.params.id, data)
    };

    // Ladda filmer när komponenten har monterats
    onMounted(async () => {
        await getMovies();
    });
</script>-->
