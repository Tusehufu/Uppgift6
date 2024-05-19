<!--<template>
    <div>
        <h1>Lägg till film</h1>
        <form @submit="onSubmit">
            <div class="mb-3">
                <label for="title" class="form-label">Title</label>
                <kn-input-group>
                    <template #prepend>
                        <button class="btn btn-primary">Hejklick</button>
                        <kn-input-group-text>@</kn-input-group-text>
                    </template>
                    <input v-model="title" type="text" class="form-control" id="title" />
                    <template #append>
                        <button class="btn btn-primary">Hejklick</button>
                    </template>
                </kn-input-group>
            </div>
            <div class="mb-3">
                <label for="releaseYear" class="form-label">Release year</label>

                <input v-model="releaseYear" type="date" class="form-control" id="releaseYear">

            </div>
            <div class="mb-3">
                <div>
                    <div class="input-group mb-3">
                        <slot name="prepend" />
                        <span class="input-group-text" id="basic-addon1">@</span>
                        <slot name="default" />
                        <slot name="append" />
                    </div>
                    <kn-range-slider :required="true"
                                     :disabled="false"
                                     :step="2"
                                     :min="1"
                                     :max="10"
                                     :value="rangeSliderValue"
                                     :labelValues="[1,3,5,7,10]"
                                     :label="'Average rating'"
                                     @change="onRangeSliderChange">
                    </kn-range-slider>
                    <button type="button" @click="averageRating = 8;componentKey++;">Ändra värde till 8</button>
                    <p>Current Values: {{ rangeSliderValue.start }} - {{rangeSliderValue.end}}</p>
                </div>
                <kn-slider-field :key="componentKey"
                                 :required="true"
                                 :use-buttons="true"
                                 :default-value="5"
                                 :disabled="true"
                                 :min="1"
                                 :max="10"
                                 v-model="averageRating"
                                 :label="'Average rating'"></kn-slider-field>
                {{averageRating}}
            </div>
            <div class="dx-field custom-height-slider">
                <div class="dx-field-label">With labels</div>
                <div class="dx-field-value">
                    <DxSlider :required="true"
                              :disabled="false"
                              :min="1"
                              :max="10"
                              v-model="averageRating"
                               />
                    {{dxValue}}
                </div>
            </div>
            <div id="app">
                <label class="labeltext">Default</label>
                <ejs-slider v-model="averageRating" 
                            :enabled="true"
                            :ticks="ticks" 
                            :min="1" 
                            :max="10"
                            :showButtons="true"></ejs-slider>
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
        </form>
    </div>
</template>

<script lang="ts" setup>
    import { ref } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router'
    import KnRangeSlider from '../../components/KnRangeSlider.vue';
    import KnInputGroup from '../../components/KnInputGroup.vue';
    import KnInputGroupText from '../../components/KnInputGroupText.vue';
    import OnRangeSliderChangedEvent from '../../types/OnRangeSliderChangedEvent';
    import RangeSliderValue from '../../types/RangeSliderValue';
    import KnSliderField from '../../components/KnSliderField.vue';
    import DxSlider from '../../components/DxSlider.vue';
    import  EjsSlider  from '../../components/EjsSlider.vue';
    import { SliderPlugin } from "@syncfusion/ej2-vue-inputs";


    const ticks = ref({
        placement: 'After',
        /*format: 'C2',*/
        largeStep: 2,
        smallStep: 1,
        showSmallTicks: true,
    });

    const router = useRouter();

    const title = ref('');
    const releaseYear = ref('');
    const averageRating = ref(5);

    const componentKey = ref(0);
    componentKey.value++;

    const rangeSliderValue = ref<RangeSliderValue>({ start: 2, end: 8 });
    const onRangeSliderChange = (event: OnRangeSliderChangedEvent) => {
        rangeSliderValue.value = event
    };
    const onSubmit = async () => {
        const data = {
            title: title.value,
            releaseYear: releaseYear.value,
            averageRating: averageRating.value,
        };

        const response = await axios.post('https://localhost:7056/api/movie', data);
        router.push({
            name: 'movie-find',
            params: {
                id: response.data
            }
        });
    };
    const label = {
        visible: true,
        position: 'top',
    };




</script>-->