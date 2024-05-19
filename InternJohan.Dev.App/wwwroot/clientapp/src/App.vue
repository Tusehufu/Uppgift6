<template>
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
        <div class="container-fluid">
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <router-link class="nav-link" to="/">Home</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" :to="{name:'bookings'}">Bokning</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" :to="{name:'sports'}">Sportevenemang</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" :to="{name:'forum'}">Forum</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" :to="{name:'about'}">Om</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" :to="{name:'createsportevent'}">Skapa Idrottsevenemang</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" :to="{name:'register'}">Registrera</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" :to="{name:'login'}">Logga in</router-link>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <div class="container">
        <router-view />
    </div>
</template>

<script>
</script>

<style lang="scss">
    @import "~bootstrap/scss/bootstrap.scss";
</style>


