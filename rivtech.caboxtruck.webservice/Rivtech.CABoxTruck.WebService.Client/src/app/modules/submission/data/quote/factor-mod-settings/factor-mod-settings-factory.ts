import { CurrencyMaskInputMode } from "ngx-currency";

export class FactorModSettingsFactory {

    SETTINGS_1: any = {
        align: 'left',
        prefix: '',
        allowNegative: false,
        precision: 2,
        nullable: true,
        min: 0.0000,
        inputMode: CurrencyMaskInputMode.NATURAL
    };

    SETTINGS_2: any = {
        align: 'left',
        prefix: '',
        allowNegative: false,
        precision: 4,
        nullable: true,
        min: 0.0000,
        inputMode: CurrencyMaskInputMode.NATURAL
    };

    SETTINGS_3: any = {
        align: 'left',
        prefix: '',
        allowNegative: false,
        precision: 6,
        nullable: true,
        min: 0.0000,
        inputMode: CurrencyMaskInputMode.NATURAL
    };

    public get(raterVersionId: number) {
        console.log(`Rater version: ${raterVersionId}`);
        var setting: any = null;

        if(raterVersionId < 3) {
            setting = this.SETTINGS_1;
        } else if (raterVersionId < 9) {
            setting = this.SETTINGS_2;
        } else {
            setting = this.SETTINGS_3;
        }

        console.log(setting);

        return setting;
    }
}