import { CurrencyMaskInputMode } from 'ngx-currency';

export const RiskSpecificsConstants = {
    maskOptions: {
        percentage: {
            align: 'left',
            allowNegative: false,
            max: 100,
            prefix: '',
            suffix: '%',
            precision: 1,
            nullable: true
        }
    },
    sections: {
        generalLiability: 12,
        cargo: 13
    }
};

