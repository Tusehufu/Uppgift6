<template>
    <div class="about">
        <h1 class="text-center">Om oss</h1>
        <h4 class="text-center">
            Vi är ett team på 3 personer som genomför detta projektet. Har ni några frågor eller andra funderingar hänvisar vi till att kontakta projektledaren.
        </h4>
        <div class="container">
            <div class="row">
                <div class="col">
                    <div class="card" style="width: 18rem;">
                        <img src="../../assets/profil.jpg" class="card-img-top" alt="profilbild">
                        <div class="card-body">
                            <h5 class="card-title">Ugglan</h5>
                            <p class="card-text">
                                Projektledare <br />
                                Fullstackutvecklare
                            </p>
                        </div>
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item"><i class="bi bi-envelope"></i>Ugglans@mail.se</li>
                            <li class="list-group-item">01000001</li>
                        </ul>
                    </div>
                </div>
                <div class="col">
                    <div class="card" style="width: 18rem;">
                        <img src="../../assets/profil.jpg" class="card-img-top" alt="profilbild">
                        <div class="card-body">
                            <h5 class="card-title">Databas Danne</h5>
                            <p class="card-text">
                                Back-end Utvecklare
                            </p>
                        </div>
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item">DbDanne@mail.se</li>
                            <li class="list-group-item">01100010</li>
                        </ul>
                    </div>
                </div>
                <div class="col">
                    <div class="card" style="width: 18rem;">
                        <img src="../../assets/profil.jpg" class="card-img-top" alt="profilbild">
                        <div class="card-body">
                            <h5 class="card-title">Layout Leif</h5>
                            <p class="card-text">
                                Front-end Utvecklare
                            </p>
                        </div>
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item">LayLeif@mail.se</li>
                            <li class="list-group-item">01100011</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';



    export default defineComponent({
        name: 'AboutView',
        components: {
        },
    });
</script>
