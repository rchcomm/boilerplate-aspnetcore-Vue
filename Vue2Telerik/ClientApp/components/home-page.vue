<template>
    <div>
        <h1>Hello, world!</h1>
        <p>Welcome to your new single-page application, built with:</p>
        <ul>
            <li><a href="https://get.asp.net/">ASP.NET Core</a> and <a href="https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx">C#</a>                    for cross-platform server-side code</li>
            <li><a href="https://vuejs.org/">Vue.js</a> for client-side code</li>
            <li><a href="https://webpack.js.org/">Webpack</a> for building and bundling client-side resources</li>
            <li><a href="http://getbootstrap.com/">Bootstrap</a> for layout and styling</li>
            <li><a href="api/SampleData/WeatherForecasts">API sample data</a> from the dotnet controller</li>
        </ul>

        <div class="col-xs-12 col-sm-6 example-col">
            <p>AutoComplete</p>
            <kendo-autocomplete :data-source="dataSourceArray"
                                :placeholder="'Your favorite sport'"
                                :separator="', '"
                                :filter="'contains'">
            </kendo-autocomplete>
        </div>
        <div class="col-xs-12 col-sm-6 example-col">
            <p>ComboBox</p>
            <kendo-combobox :data-source="dataSourceArray"
                            :filter="'contains'"
                            :index="0">
            </kendo-combobox>
        </div>

        <div class="col-xs-12 col-sm-6 example-col">
            <p>DropDownList</p>
            <kendo-dropdownlist :data-source=dataSourceArray
                                :index="0">
            </kendo-dropdownlist>
        </div>

        <div class="col-xs-12 col-sm-6 example-col">
            <p>MultiSelect</p>
            <kendo-multiselect :data-source=dataSourceArray
                               :filter="'contains'">
            </kendo-multiselect>
        </div>
        <div class="col-xs-12 col-sm-6 example-col">
            <kendo-calendar :value="new Date()" v-on:change="onChange"></kendo-calendar>
        </div>

        <div class="col-xs-12 col-sm-6 example-col">

            <kendo-grid :data-source="localDataSource"
                        v-on:databinding="onDataBinding"
                        v-on:databound="onDataBound">
                <kendo-grid-column field="ProductID" title="ID" :width="40"></kendo-grid-column>
                <kendo-grid-column field="ProductName"></kendo-grid-column>
                <kendo-grid-column field="UnitPrice" title="Unit Price" :width="120" :format="'{0:c}'"></kendo-grid-column>
                <kendo-grid-column field="UnitsInStock" title="Units In Stock" :width="120"></kendo-grid-column>
                <kendo-grid-column field="Discontinued" :width="120"></kendo-grid-column>
            </kendo-grid>
        </div>
        <div class="col-xs-12 col-sm-6 example-col">
            <kendo-datasource ref="datasource1"
                              :transport-read-url="'https://demos.telerik.com/kendo-ui/service/Products'"
                              :transport-read-data-type="'jsonp'"
                              :transport-update-url="'https://demos.telerik.com/kendo-ui/service/Products/Update'"
                              :transport-update-data-type="'jsonp'"
                              :transport-destroy-url="'https://demos.telerik.com/kendo-ui/service/Products/Destroy'"
                              :transport-destroy-data-type="'jsonp'"
                              :transport-create-url="'https://demos.telerik.com/kendo-ui/service/Products/Create'"
                              :transport-create-data-type="'jsonp'"
                              :transport-parameter-map="parameterMap"
                              :schema-model-id="'ProductID'"
                              :schema-model-fields="schemaModelFields"
                              :batch='true'
                              :page-size='20'>
            </kendo-datasource>

            <kendo-grid :height="600" :data-source-ref="'datasource1'" :pageable='true' :editable="'inline'" :selectable='true' :groupable='true' :sortable='true' :toolbar="['create']">
                <kendo-grid-column field="ProductName"></kendo-grid-column>
                <kendo-grid-column field="UnitPrice" title="Unit Price" :width="120" :format="'{0:c}'"></kendo-grid-column>
                <kendo-grid-column field="UnitsInStock" title="Units In Stock" :width="120"></kendo-grid-column>
                <kendo-grid-column field="Discontinued" :width="120" :editor="customBoolEditor"></kendo-grid-column>
                <kendo-grid-column :command="['edit', 'destroy']" title="&nbsp;" width="170px"></kendo-grid-column>
            </kendo-grid>
        </div>

        <div class="col-xs-12 col-sm-12 example-col">
            <p>Button</p>
            <kendo-button @click="onButtonClick">Default</kendo-button>
            <kendo-button class="k-primary" @click="onButtonClick">Primary</kendo-button>
            <kendo-button :disabled="true" @click="onButtonClick">Disabled</kendo-button>
        </div>

        <div class="col-xs-12 col-sm-12 example-col">
            <p>ButtonGroup</p>
            <kendo-buttongroup>
                <kendo-buttongroup-button>Option A</kendo-buttongroup-button>
                <kendo-buttongroup-button>Option B</kendo-buttongroup-button>
                <kendo-buttongroup-button>Option C</kendo-buttongroup-button>
            </kendo-buttongroup>
        </div>

        <div class="col-xs-12 col-sm-12 example-col">
            <p>ToolBar</p>
            <kendo-toolbar>
                <kendo-toolbar-item type="button" text="Button"></kendo-toolbar-item>
                <kendo-toolbar-item type="button" text="Toggle Button" :togglable="true"></kendo-toolbar-item>
                <kendo-toolbar-item type="splitButton" text="Insert" :menu-buttons="[
                { text: 'Insert above', icon: 'insert-up' },
                { text: 'Insert between', icon: 'insert-middle' },
                { text: 'Insert below', icon: 'insert-down' }]">
                </kendo-toolbar-item>
                <kendo-toolbar-item type="separator"></kendo-toolbar-item>
                <kendo-toolbar-item type="buttonGroup" :buttons="[
                { icon: 'align-left', text: 'Left', togglable: true, group: 'text-align' },
                { icon: 'align-center', text: 'Center', togglable: true, group: 'text-align' },
                { icon: 'align-right', text: 'Right', togglable: true, group: 'text-align' }]">
                </kendo-toolbar-item>
                <kendo-toolbar-item type="button" text="Action" overflow="always"></kendo-toolbar-item>
                <kendo-toolbar-item type="button" text="Another Action" overflow="always"></kendo-toolbar-item>
                <kendo-toolbar-item type="button" text="Something else here" overflow="always"></kendo-toolbar-item>
            </kendo-toolbar>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            schemaModelFields: {
                ProductID: { editable: false, nullable: true },
                ProductName: { validation: { required: true } },
                UnitPrice: { type: 'number', validation: { required: true, min: 1 } },
                Discontinued: { type: 'boolean' },
                UnitsInStock: { type: 'number', validation: { min: 0, required: true } }
            },
            dataSourceArray: [
                'Football',
                'Tennis',
                'Basketball',
                'Baseball',
                'Cricket',
                'Field Hockey',
                'Volleyball'
            ],
            localDataSource: [{
                "ProductID": 1,
                "ProductName": "Chai",
                "UnitPrice": 18,
                "UnitsInStock": 39,
                "Discontinued": false,
            },
            {
                "ProductID": 2,
                "ProductName": "Chang",
                "UnitPrice": 17,
                "UnitsInStock": 40,
                "Discontinued": false,
            },
            {
                "ProductID": 3,
                "ProductName": "Aniseed Syrup",
                "UnitPrice": 10,
                "UnitsInStock": 13,
                "Discontinued": false,
            },
            {
                "ProductID": 4,
                "ProductName": "Chef Anton's Cajun Seasoning",
                "UnitPrice": 22,
                "UnitsInStock": 53,
                "Discontinued": false,
            },
            {
                "ProductID": 5,
                "ProductName": "Chef Anton's Gumbo Mix",
                "UnitPrice": 21.35,
                "UnitsInStock": 0,
                "Discontinued": true,
            },
            {
                "ProductID": 6,
                "ProductName": "Grandma's Boysenberry Spread",
                "UnitPrice": 25,
                "UnitsInStock": 120,
                "Discontinued": false,
            },
            {
                "ProductID": 7,
                "ProductName": "Uncle Bob's Organic Dried Pears",
                "UnitPrice": 30,
                "UnitsInStock": 15,
                "Discontinued": false,
            }
            ]
        }
        },
    methods: {
        customBoolEditor: function (container, options) {
            kendo.jQuery('<input class="k-checkbox" type="checkbox" name="Discontinued" data-type="boolean" data-bind="checked:Discontinued">').appendTo(container)
            kendo.jQuery('<label class="k-checkbox-label">&#8203;</label>').appendTo(container)
        },
        parameterMap: function (options, operation) {
            if (operation !== 'read' && options.models) {
                return { models: kendo.stringify(options.models) }
            }
        },
        onDataBinding: function (ev) {
            console.log("Grid is about to be bound!");
        },
        onDataBound: function (ev) {
            console.log("Grid is now bound!");
        },
        onChange(ev) {
            console.log("date changed to: " + ev.sender.value());
        },
        onButtonClick: function (ev) {
            console.log("Button clicked!");
        }
    }
}
</script>

<style>
</style>
