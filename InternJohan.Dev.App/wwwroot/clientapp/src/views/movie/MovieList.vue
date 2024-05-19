<template>
  <div>
      <h1>Filmer</h1>
      <ul>
          <li v-for="movie in movies">{{movie}} |  <router-link class="nav-link" :to="{
            name: 'movie-update',
            params: {
                id: movie.id
            }
        }">Ändra</router-link></li>
      </ul>
  </div>
</template>

<script setup lang="ts">
    import { onMounted, ref } from 'vue';
    import axios from 'axios';
    //import { useRouter } from 'vue-router'

    //const router = useRouter()
    const movies = ref([]);


    const getMovies = async () => {
        const response = await axios.get('https://localhost:7056/api/movie');
        movies.value = response.data;
    };

    onMounted(async () => {
       await getMovies();
    });

    //const edit = (movieId: number) => {
    //    router.push({
    //        name: 'movie-update',
    //        params: {
    //            id: movieId
    //        }
    //    })
    //}
</script>
