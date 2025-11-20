import { MonthModel } from "../models/month-model";

export class GenerateMontsToFilter {
    monthModel: MonthModel[];

    fillMonthsToFilter(nowDate: Date): MonthModel[] {
        this.monthModel = [];

        for (let i = -2; i < 12; i++) {
            const date = new Date(nowDate);
            date.setMonth(nowDate.getMonth() - i);

            this.monthModel.push({
                month: date.toLocaleString('pt-BR', { month: 'long', year: 'numeric' }).replace(/^\w/, c => c.toUpperCase()),
                referenceDate: date,
            });
        }

        return this.monthModel;
    }

}