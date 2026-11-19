import { api } from './api.js';
import { openForm } from './form.js';
import { openExpediente } from './expediente.js';
import {
  badge, confirmDialog, formatDate, formatDateTime, formatHour, formatTime,
  fullName, h, money, personCell, toast,
} from './ui.js';

const toOptions = labels => Object.entries(labels).map(([value, label]) => ({ value, label }));

const DIAS = { Lunes: 'Lunes', Martes: 'Martes', Miercoles: 'Miércoles', Jueves: 'Jueves', Viernes: 'Viernes', Sabado: 'Sábado', Domingo: 'Domingo' };
const ORDEN_DIAS = Object.keys(DIAS);
const METODOS = { Efectivo: 'Efectivo', Tarjeta: 'Tarjeta', Transferencia: 'Transferencia' };
export const ESTADOS = {
  Pendiente: { tone: 'warn', icon: 'clock' },
  Confirmado: { tone: 'info', icon: 'calendar' },
  Completado: { tone: 'good', icon: 'check' },
  Cancelado: { tone: 'danger', icon: 'close' },
};

export const estadoBadge = estado => badge(estado, ESTADOS[estado].tone, ESTADOS[estado].icon);
const activoBadge = activo => (activo ? badge('Activo', 'good', 'check') : badge('Inactivo', 'neutral', 'close'));
const activaBadge = activa => (activa ? badge('Activa', 'good', 'check') : badge('Inactiva', 'neutral', 'close'));

const byId = (list, id) => list?.find(item => item.id === id);
const clienteDe = (ctx, turno) => byId(ctx.clientes, turno?.clienteId);
const turnoLabel = (ctx, turno) => (turno ? `${fullName(clienteDe(ctx, turno))} · ${formatDateTime(turno.fechaHoraInicio)}` : '—');
export const pagadoTurno = (pagos, turnoId, excepto) => (pagos || [])
  .filter(p => p.turnoId === turnoId && p.id !== excepto)
  .reduce((suma, p) => suma + p.monto, 0);
export const saldoTurno = (pagos, turno, excepto) => turno.precio - pagadoTurno(pagos, turno.id, excepto);
const abierto = turno => turno.estado === 'Pendiente' || turno.estado === 'Confirmado';

const activeOptions = (list, flag, current, label) => list
  .filter(item => item[flag] !== false || item.id === current)
  .map(item => ({ value: item.id, label: label(item) }));

const emailField = { name: 'email', label: 'Correo', type: 'email', max: 150, placeholder: 'nombre@correo.com' };
const telefonoField = { name: 'telefono', label: 'Teléfono', type: 'tel', required: true, max: 20, placeholder: '809-000-0000' };
const nameFields = [
  { name: 'nombre', label: 'Nombre', required: true, max: 100 },
  { name: 'apellido', label: 'Apellido', required: true, max: 100 },
];

export const MODULES = [
  {
    key: 'turnos', title: 'Turnos', singular: 'turno', newLabel: 'Nuevo turno', icon: 'calendar', group: 'Agenda',
    description: 'Agenda de turnos, confirmación, cobro y cancelación.',
    emptyText: 'Agenda el primer turno para empezar a llenar la agenda.',
    searchPlaceholder: 'Buscar por cliente, técnica o servicio…',
    refs: ['clientes', 'tecnicas', 'servicios', 'empleados', 'pagos'],
    initialSort: (a, b) => b.fechaHoraInicio.localeCompare(a.fechaHoraInicio),
    canEdit: abierto,
    filters: [
      { name: 'estado', label: 'Todos los estados', value: r => r.estado, options: toOptions(Object.fromEntries(Object.keys(ESTADOS).map(e => [e, e]))) },
      { name: 'tecnica', label: 'Todas las técnicas', value: r => r.tecnicaId, options: c => c.tecnicas.map(t => ({ value: t.id, label: fullName(t) })) },
    ],
    describe: (r, c) => `el turno de ${turnoLabel(c, r)}`,
    columns: [
      { label: 'Fecha', value: r => r.fechaHoraInicio, render: r => h('div', { class: 'nowrap' }, h('strong', {}, formatDate(r.fechaHoraInicio)), h('div', { class: 'muted small' }, `${formatTime(r.fechaHoraInicio)} – ${formatTime(r.fechaHoraFin)}`)) },
      { label: 'Cliente', value: (r, c) => fullName(clienteDe(c, r)), render: (r, c) => personCell(clienteDe(c, r), r.observacion) },
      { label: 'Servicio', value: (r, c) => byId(c.servicios, r.servicioId)?.nombre },
      { label: 'Técnica', value: (r, c) => fullName(byId(c.tecnicas, r.tecnicaId)) },
      { label: 'Atendió', value: (r, c) => fullName(byId(c.empleados, r.empleadoId)) },
      { label: 'Estado', value: r => r.estado, render: r => estadoBadge(r.estado) },
      { label: 'Precio', num: true, value: r => r.precio, render: r => money(r.precio) },
      { label: 'Saldo', num: true, value: (r, c) => saldoTurno(c.pagos, r), render: (r, c) => money(saldoTurno(c.pagos, r)) },
    ],
    fields: [
      { name: 'clienteId', label: 'Cliente', type: 'select', numeric: true, required: true, span: 2, options: c => c.clientes.map(x => ({ value: x.id, label: `${fullName(x)} · ${x.telefono}` })) },
      { name: 'servicioId', label: 'Servicio', type: 'select', numeric: true, required: true, options: (c, r) => activeOptions(c.servicios, 'activo', r?.servicioId, s => `${s.nombre} · ${s.duracionMinutos} min · ${money(s.precio)}`) },
      { name: 'tecnicaId', label: 'Técnica', type: 'select', numeric: true, required: true, options: (c, r) => activeOptions(c.tecnicas, 'activa', r?.tecnicaId, t => `${fullName(t)} · ${t.especialidad}`) },
      { name: 'empleadoId', label: 'Atendido por', type: 'select', numeric: true, required: true, span: 2, options: (c, r) => activeOptions(c.empleados, 'activo', r?.empleadoId, e => `${fullName(e)} · ${e.cargo}`) },
      { name: 'fechaHoraInicio', label: 'Fecha y hora', type: 'slots', required: true },
      { name: 'observacion', label: 'Observación', type: 'textarea', max: 500, span: 2, placeholder: 'Diseño, color o detalles que pidió el cliente' },
    ],
    actions: (row, ctx, { reload }) => {
      const acciones = [];
      if (row.estado === 'Pendiente') {
        acciones.push({ icon: 'check', label: 'Confirmar turno', tone: 'good', onClick: () => cambiarEstado(row, 'Confirmado', reload) });
      }
      if (abierto(row)) {
        acciones.push({ icon: 'done', label: 'Marcar como completado', tone: 'good', onClick: () => cambiarEstado(row, 'Completado', reload) });
      }
      if (row.estado !== 'Cancelado' && saldoTurno(ctx.pagos, row) > 0) {
        acciones.push({
          icon: 'money', label: 'Registrar pago',
          onClick: () => openForm(moduleByKey('pagos'), { prefill: { turnoId: row.id, monto: saldoTurno(ctx.pagos, row) }, onSaved: reload }),
        });
      }
      if (abierto(row)) {
        acciones.push({
          icon: 'close', label: 'Cancelar turno', tone: 'danger',
          onClick: async () => {
            const ok = await confirmDialog({
              title: 'Cancelar turno',
              message: `Se cancelará el turno de ${turnoLabel(ctx, row)}. Un turno cancelado no se puede reactivar.`,
              confirmLabel: 'Cancelar turno',
            });
            if (ok) cambiarEstado(row, 'Cancelado', reload);
          },
        });
      }
      return acciones;
    },
  },
  {
    key: 'pagos', title: 'Pagos', singular: 'pago', newLabel: 'Registrar pago', icon: 'card', group: 'Agenda',
    description: 'Cobros de turnos. Un pago nunca puede superar el saldo pendiente.',
    emptyText: 'Registra un pago desde esta página o desde un turno.',
    searchPlaceholder: 'Buscar por cliente o referencia…',
    refs: ['turnos', 'clientes'],
    initialSort: (a, b) => b.fechaPago.localeCompare(a.fechaPago),
    filters: [{ name: 'metodoPago', label: 'Todos los métodos', value: r => r.metodoPago, options: toOptions(METODOS) }],
    describe: (r, c) => `el pago de ${money(r.monto)} de ${fullName(clienteDe(c, byId(c.turnos, r.turnoId)))}`,
    columns: [
      { label: 'Cliente', value: (r, c) => fullName(clienteDe(c, byId(c.turnos, r.turnoId))), render: (r, c) => personCell(clienteDe(c, byId(c.turnos, r.turnoId))) },
      { label: 'Turno', value: (r, c) => byId(c.turnos, r.turnoId)?.fechaHoraInicio, render: (r, c) => h('span', { class: 'nowrap' }, formatDateTime(byId(c.turnos, r.turnoId)?.fechaHoraInicio)) },
      { label: 'Fecha de pago', value: r => r.fechaPago, render: r => h('span', { class: 'nowrap' }, formatDateTime(r.fechaPago)) },
      { label: 'Método', value: r => r.metodoPago },
      { label: 'Referencia', value: r => r.referencia },
      { label: 'Monto', num: true, value: r => r.monto, render: r => money(r.monto) },
    ],
    fields: [
      {
        name: 'turnoId', label: 'Turno', type: 'select', numeric: true, required: true, span: 2,
        options: (c, r) => {
          const pagos = c.pagos || [];
          return c.turnos
            .filter(x => x.estado !== 'Cancelado' && (saldoTurno(pagos, x, r?.id) > 0 || x.id === r?.turnoId))
            .map(x => ({ value: x.id, label: `${turnoLabel(c, x)} · saldo ${money(saldoTurno(pagos, x, r?.id))}` }));
        },
      },
      { name: 'monto', label: 'Monto', type: 'number', required: true, min: 0.01, maxValue: 9999999.99, step: 0.01 },
      { name: 'metodoPago', label: 'Método de pago', type: 'select', required: true, options: toOptions(METODOS) },
      { name: 'referencia', label: 'Referencia', max: 100, span: 2, placeholder: 'Número de voucher o transferencia' },
    ],
  },
  {
    key: 'clientes', title: 'Clientes', singular: 'cliente', newLabel: 'Nuevo cliente', icon: 'users', group: 'Personas',
    description: 'Datos de contacto e historial de cada cliente.',
    emptyText: 'Registra el primer cliente para agendarle un turno.',
    searchPlaceholder: 'Buscar por nombre, teléfono o correo…',
    refs: ['turnos'],
    initialSort: (a, b) => fullName(a).localeCompare(fullName(b), 'es'),
    describe: r => fullName(r),
    columns: [
      { label: 'Cliente', value: r => fullName(r), render: r => personCell(r, r.email) },
      { label: 'Teléfono', value: r => r.telefono, render: r => h('span', { class: 'nowrap' }, r.telefono) },
      { label: 'Cliente desde', value: r => r.fechaRegistro, render: r => h('span', { class: 'nowrap' }, formatDate(r.fechaRegistro)) },
      { label: 'Turnos', num: true, value: (r, c) => c.turnos.filter(t => t.clienteId === r.id).length },
    ],
    fields: [
      ...nameFields,
      telefonoField,
      emailField,
    ],
    actions: row => [{ icon: 'folder', label: 'Ver historial', onClick: () => openExpediente(row, { estadoBadge, saldoTurno }) }],
  },
  {
    key: 'tecnicas', title: 'Técnicas', singular: 'técnica', newLabel: 'Nueva técnica', icon: 'sparkle', group: 'Personas',
    description: 'Profesionales que realizan los servicios.',
    emptyText: 'Registra una técnica y luego asígnale un horario.',
    searchPlaceholder: 'Buscar por nombre o especialidad…',
    initialSort: (a, b) => fullName(a).localeCompare(fullName(b), 'es'),
    describe: r => fullName(r),
    columns: [
      { label: 'Técnica', value: r => fullName(r), render: r => personCell(r, r.email) },
      { label: 'Especialidad', value: r => r.especialidad },
      { label: 'Teléfono', value: r => r.telefono, render: r => h('span', { class: 'nowrap' }, r.telefono) },
      { label: 'Estado', value: r => (r.activa ? 'Activa' : 'Inactiva'), render: r => activaBadge(r.activa) },
    ],
    fields: [
      ...nameFields,
      { name: 'especialidad', label: 'Especialidad', required: true, max: 100, placeholder: 'Acrílico, gel, pedicure…' },
      telefonoField,
      emailField,
      { name: 'activa', label: 'Estado', type: 'checkbox', checkLabel: 'Recibe turnos' },
    ],
  },
  {
    key: 'empleados', title: 'Empleados', singular: 'empleado', newLabel: 'Nuevo empleado', icon: 'user', group: 'Personas',
    description: 'Personal que registra y atiende los turnos.',
    emptyText: 'Registra un empleado para indicar quién atendió cada turno.',
    searchPlaceholder: 'Buscar por nombre o cargo…',
    initialSort: (a, b) => fullName(a).localeCompare(fullName(b), 'es'),
    describe: r => fullName(r),
    columns: [
      { label: 'Empleado', value: r => fullName(r), render: r => personCell(r, r.email) },
      { label: 'Cargo', value: r => r.cargo },
      { label: 'Teléfono', value: r => r.telefono, render: r => h('span', { class: 'nowrap' }, r.telefono) },
      { label: 'Estado', value: r => (r.activo ? 'Activo' : 'Inactivo'), render: r => activoBadge(r.activo) },
    ],
    fields: [
      ...nameFields,
      { name: 'cargo', label: 'Cargo', required: true, max: 100, placeholder: 'Recepcionista' },
      telefonoField,
      emailField,
      { name: 'activo', label: 'Estado', type: 'checkbox', checkLabel: 'Activo' },
    ],
  },
  {
    key: 'horarios', title: 'Horarios', singular: 'horario', newLabel: 'Nuevo horario', icon: 'clock', group: 'Personas',
    description: 'Bloques semanales en los que cada técnica recibe turnos.',
    emptyText: 'Sin horarios no se pueden agendar turnos.',
    searchPlaceholder: 'Buscar por técnica o día…',
    refs: ['tecnicas'],
    initialSort: (a, b) => a.tecnicaId - b.tecnicaId
      || ORDEN_DIAS.indexOf(a.diaSemana) - ORDEN_DIAS.indexOf(b.diaSemana)
      || a.horaInicio.localeCompare(b.horaInicio),
    filters: [{ name: 'dia', label: 'Todos los días', value: r => r.diaSemana, options: toOptions(DIAS) }],
    describe: (r, c) => `el horario de ${fullName(byId(c.tecnicas, r.tecnicaId))} del ${DIAS[r.diaSemana]}`,
    columns: [
      { label: 'Técnica', value: (r, c) => fullName(byId(c.tecnicas, r.tecnicaId)), render: (r, c) => personCell(byId(c.tecnicas, r.tecnicaId)) },
      { label: 'Día', value: r => DIAS[r.diaSemana] },
      { label: 'Inicio', value: r => r.horaInicio, render: r => formatHour(r.horaInicio) },
      { label: 'Fin', value: r => r.horaFin, render: r => formatHour(r.horaFin) },
      { label: 'Estado', value: r => (r.disponible ? 'Disponible' : 'No disponible'), render: r => (r.disponible ? badge('Disponible', 'good', 'check') : badge('No disponible', 'neutral', 'close')) },
    ],
    fields: [
      { name: 'tecnicaId', label: 'Técnica', type: 'select', numeric: true, required: true, options: (c, r) => activeOptions(c.tecnicas, 'activa', r?.tecnicaId, fullName) },
      { name: 'diaSemana', label: 'Día', type: 'select', required: true, options: toOptions(DIAS) },
      { name: 'horaInicio', label: 'Hora de inicio', type: 'time', required: true },
      { name: 'horaFin', label: 'Hora de fin', type: 'time', required: true },
      { name: 'disponible', label: 'Estado', type: 'checkbox', checkLabel: 'Disponible para turnos' },
    ],
  },
  {
    key: 'servicios', title: 'Servicios', singular: 'servicio', newLabel: 'Nuevo servicio', icon: 'tag', group: 'Catálogos',
    description: 'Servicios que ofrece el centro, con su precio y duración.',
    emptyText: 'Agrega un servicio para poder agendar turnos.',
    searchPlaceholder: 'Buscar por nombre…',
    initialSort: (a, b) => a.nombre.localeCompare(b.nombre, 'es'),
    describe: r => r.nombre,
    columns: [
      { label: 'Servicio', value: r => r.nombre },
      { label: 'Descripción', value: r => r.descripcion, render: r => h('span', { class: 'cell-clip', title: r.descripcion || '' }, r.descripcion || '—') },
      { label: 'Duración', num: true, value: r => r.duracionMinutos, render: r => `${r.duracionMinutos} min` },
      { label: 'Estado', value: r => (r.activo ? 'Activo' : 'Inactivo'), render: r => activoBadge(r.activo) },
      { label: 'Precio', num: true, value: r => r.precio, render: r => money(r.precio) },
    ],
    fields: [
      { name: 'nombre', label: 'Nombre', required: true, max: 100, placeholder: 'Manicure en gel' },
      { name: 'activo', label: 'Estado', type: 'checkbox', checkLabel: 'Activo' },
      { name: 'precio', label: 'Precio', type: 'number', required: true, min: 0.01, maxValue: 9999999.99, step: 0.01 },
      { name: 'duracionMinutos', label: 'Duración (minutos)', type: 'number', required: true, min: 5, maxValue: 480, step: 5 },
      { name: 'descripcion', label: 'Descripción', type: 'textarea', max: 500, span: 2 },
    ],
  },
];

export const moduleByKey = key => MODULES.find(m => m.key === key);

async function cambiarEstado(row, estado, reload) {
  try {
    await api.cambiarEstadoTurno(row.id, estado);
    toast(`Turno ${estado.toLowerCase()}.`);
    reload();
  } catch (error) {
    toast(error.message, 'error');
  }
}
