<!--<template>
    <div>
        <h1>Ta bort en film!</h1>
        <form @submit="onSubmit">
            <div class="mb-3">
                <label for="movieId" class="form-label">Movie Id</label>
                <input v-model="movieId" type="text" class="form-control" id="movieId">
            </div>
            <button type="submit" class="btn btn-danger">Submit</button>
            <div class="m-3">
                <p>ColorPicker with gradient and palette</p>
                <kn-color-picker :required="false"
                                 :disabled="false"
                                 v-model="whiteColor">
                </kn-color-picker>
                <button type="button" @click="whiteColor = '#000000'">Ändra till svart</button>
                {{whiteColor}}
            </div>
            <div class="dx-fieldset">
                <div class="dx-field">
                    <div class="dx-field-label">
                    Default mode
                    </div>
                        
                    <div class="dx-field-value">
                        <DxColorBox 
                                    :required="true"
                                    v-model="dxColor"
                                    label="dxColorLabel"/>
                                    
                        {{dxColor}}
                    </div>
                </div>
            </div>
        </form>
    </div>
    <div class="row">
        <KnAccordion>
            <KnAccordionItem header="Hej">
                <p>Tjena</p>
            </KnAccordionItem>
            <KnAccordionItem>
                <p>tjuna</p>
            </KnAccordionItem>
        </KnAccordion>
        <hr />
        <KnAccordion>
            <KnAccordionItem>
                <p>Tjena</p>
            </KnAccordionItem>
            <KnAccordionItem>
                <p>tjuna</p>
            </KnAccordionItem>
        </KnAccordion>
    </div>
</template>

<script setup lang="ts">
    import { ref } from 'vue';
    import axios from 'axios';
    import KnColorPicker from '../../components/KnColorPicker.vue';
    import DxColorBox from '../../components/DxColorBox.vue';
    import KnAccordion from '../../components/KnAccordion.vue';
    import KnAccordionItem from '../../components/KnAccordionItem.vue';



    const whiteColor = ref<string | null>('#ff0000c2');
    const movieId = ref(null);

    const onSubmit = async () => {
        if (!whiteColor.value) {
            alert('Du gjorde fel');
            return
        }
        const response = await axios.delete('https://localhost:7056/api/Movie/' + movieId.value);
        console.log(response.data);
    };

    const dxColor = ref('');
</script>-->
